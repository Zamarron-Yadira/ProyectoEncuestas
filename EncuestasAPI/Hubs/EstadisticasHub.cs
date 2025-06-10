using EncuestasAPI.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EncuestasAPI.Hubs
{
	public class EstadisticasHub:Hub
	{
	
		public override async Task OnConnectedAsync()
		{
			var nombre = Context.User.Identity?.Name;
			Console.WriteLine($"✅ Usuario conectado: {nombre}");

			var claims = Context.User.Claims.Select(c => $"{c.Type}: {c.Value}");
			foreach (var claim in claims)
				Console.WriteLine("🔍 " + claim);

			// Guardar usuario
			if (!string.IsNullOrEmpty(nombre))
				UsuariosActivosStore.AgregarUsuario(Context.ConnectionId, nombre);
			
			// Notificar a todos
			await Clients.All.SendAsync("UsuariosActivos", UsuariosActivosStore.ObtenerUsuariosActivos());
			await Clients.All.SendAsync("ActualizarEstadisticas");

			await base.OnConnectedAsync();
		}


		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var connectionId = Context.ConnectionId;

			UsuariosActivosStore.RemoverUsuario(connectionId);

			// Notificar a todos
			await Clients.All.SendAsync("UsuariosActivos", UsuariosActivosStore.ObtenerUsuariosActivos());
			await Clients.All.SendAsync("ActualizarEstadisticas");
			await base.OnDisconnectedAsync(exception);
		}

		public Task<List<string>> GetUsuariosActivos()
		{
			return Task.FromResult(UsuariosActivosStore.ObtenerUsuariosActivos());
		}

	}
}
