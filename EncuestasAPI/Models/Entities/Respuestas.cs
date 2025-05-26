using System;
using System.Collections.Generic;

namespace EncuestasAPI.Models.Entities;

public partial class Respuestas
{
    public int Id { get; set; }

    public int IdEncuesta { get; set; }

    public int IdUsuarioAplicador { get; set; }

    public string NombreAlumno { get; set; } = null!;

    public string NumControlAlumno { get; set; } = null!;

    public DateTime FechaAplicacion { get; set; }

    public virtual ICollection<Detallerespuestas> Detallerespuestas { get; set; } = new List<Detallerespuestas>();

    public virtual Encuestas IdEncuestaNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioAplicadorNavigation { get; set; } = null!;
}
