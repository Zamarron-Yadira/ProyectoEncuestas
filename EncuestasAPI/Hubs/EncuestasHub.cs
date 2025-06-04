using EncuestasAPI.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace EncuestasAPI.Hubs
{
	public class EncuestasHub : Hub
	{
		public static ConcurrentDictionary<string, UsuarioEncuestaInfo> UsuariosConectados = new();

		public async Task UnirseEncuesta(string nombreUsuario, int idEncuesta)
		{
			var info = new UsuarioEncuestaInfo
			{
				NombreUsuario = nombreUsuario,
				IdEncuesta = idEncuesta
			};

			UsuariosConectados[Context.ConnectionId] = info;

			await Clients.All.SendAsync("UsuarioAsignado", UsuariosConectados.Values.ToList());
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			UsuariosConectados.TryRemove(Context.ConnectionId, out _);
			await Clients.All.SendAsync("UsuarioAsignado", UsuariosConectados.Values.ToList());

			await base.OnDisconnectedAsync(exception);
		}
	}

}
