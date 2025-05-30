using System;
using System.Collections.Generic;

namespace EncuestasAPI.Models.Entities;

public partial class Usuarios
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public int EsAdmin { get; set; }

    public virtual ICollection<Encuestas> Encuestas { get; set; } = new List<Encuestas>();

    public virtual ICollection<Respuestas> Respuestas { get; set; } = new List<Respuestas>();
}
