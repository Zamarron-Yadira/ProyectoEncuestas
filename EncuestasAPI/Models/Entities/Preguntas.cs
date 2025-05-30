using System;
using System.Collections.Generic;

namespace EncuestasAPI.Models.Entities;

public partial class Preguntas
{
    public int IdPregunta { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdEncuesta { get; set; }

    public int NumeroPregunta { get; set; }

    public virtual ICollection<Detallerespuestas> Detallerespuestas { get; set; } = new List<Detallerespuestas>();

    public virtual Encuestas IdEncuestaNavigation { get; set; } = null!;
}
