using EncuestasAPI.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace EncuestasAPI.Hubs
{
	public class EncuestasHub : Hub
	{
		private static readonly ConcurrentDictionary<string, string> usuariosEncuesta = new();

		private readonly IHubContext<EstadisticasHub> _estadisticasHub;

		public EncuestasHub(IHubContext<EstadisticasHub> estadisticasHub)
		{
			_estadisticasHub = estadisticasHub;
		}

		public async Task UnirseEncuesta(string idEncuesta, string nombreAlumno)
		{
			var clave = $"{Context.ConnectionId}:{idEncuesta}";
			usuariosEncuesta[clave] = nombreAlumno;

			await Clients.All.SendAsync("AlumnoEnEncuesta", nombreAlumno, idEncuesta);

			var usuariosActivos = usuariosEncuesta.Values.Distinct().ToList();
			await _estadisticasHub.Clients.All.SendAsync("UsuariosActivos", usuariosActivos);
			await _estadisticasHub.Clients.All.SendAsync("ActualizarEstadisticas");
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var clavesARemover = usuariosEncuesta.Keys
				.Where(k => k.StartsWith(Context.ConnectionId + ":"))
				.ToList();

			foreach (var clave in clavesARemover)
			{
				usuariosEncuesta.TryRemove(clave, out _);
			}

			var usuariosActivos = usuariosEncuesta.Values.Distinct().ToList();
			await _estadisticasHub.Clients.All.SendAsync("UsuariosActivos", usuariosActivos);
			await _estadisticasHub.Clients.All.SendAsync("ActualizarEstadisticas");

			await base.OnDisconnectedAsync(exception);
		}

	}

}