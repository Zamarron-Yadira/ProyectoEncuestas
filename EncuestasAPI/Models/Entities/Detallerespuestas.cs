using System;
using System.Collections.Generic;

namespace EncuestasAPI.Models.Entities;

public partial class Detallerespuestas
{
    public int Id { get; set; }

    public int IdRespuesta { get; set; }

    public int IdPregunta { get; set; }

    public int ValorEvaluacion { get; set; }

    public virtual Preguntas IdPreguntaNavigation { get; set; } = null!;

    public virtual Respuestas IdRespuestaNavigation { get; set; } = null!;
}
