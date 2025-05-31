using PRN232_SE1728_A01_FE.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SE1728_A01_FE.Services.Interfaces
{
	public interface ISystemAccountService
	{
		Task<List<SystemAccountDTO>> GetListAccount();
		Task<SystemAccountDTO> GetSystemAccountAsync(string email, string password);
		Task<SystemAccountDTO> GetAccoutnProfile(short id);
		Task<bool> DeleteAccount(short id);
		Task<bool> CreateAccount(SystemAccountDTO account);
		Task<bool> UpdateAccount(SystemAccountDTO account);
		Task<bool> UpdateProfile(SystemAccountDTO profile);
		Task<List<SystemAccountDTO>> SearchByName(string name);
	}
}
