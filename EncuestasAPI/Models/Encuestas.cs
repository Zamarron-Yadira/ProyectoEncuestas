using System;
using System.Collections.Generic;

namespace EncuestasAPI.Models;

public partial class Encuestas
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Titulo { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Preguntas> Preguntas { get; set; } = new List<Preguntas>();

    public virtual ICollection<Respuestas> Respuestas { get; set; } = new List<Respuestas>();
}
