using EncuestasAPI.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EncuestasAPI.Hubs
{
	public class EstadisticasHub:Hub
	{
		public Task RegistrarAlumno(string nombre)
		{
			if (!string.IsNullOrEmpty(nombre))
			{
				UsuariosActivosStore.AgregarUsuario(Context.ConnectionId, nombre);
			}

			// Notificar usuarios activos y estadísticas
			return Task.WhenAll(
				Clients.All.SendAsync("UsuariosActivos", UsuariosActivosStore.ObtenerUsuariosActivos()),
				Clients.All.SendAsync("ActualizarEstadisticas")
			);
		}


		

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			UsuariosActivosStore.RemoverUsuario(Context.ConnectionId);

			await Task.WhenAll(
				Clients.All.SendAsync("UsuariosActivos", UsuariosActivosStore.ObtenerUsuariosActivos()),
				Clients.All.SendAsync("ActualizarEstadisticas")
			);

			await base.OnDisconnectedAsync(exception);
		}

		public Task<List<string>> GetUsuariosActivos()
		{
			return Task.FromResult(UsuariosActivosStore.ObtenerUsuariosActivos());
		}

	}
}

/*public override async Task OnConnectedAsync()
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
		*/