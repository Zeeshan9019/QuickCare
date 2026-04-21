using Common_QuickCare.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Common_QuickCare.Controllers
{
    public class BaseController : Controller
    {
        protected readonly CommonRepository _commonRepository;

        public BaseController(CommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public int UserId
        {
            get
            {
                var value = User.FindFirst("UserId")?.Value;
                return int.TryParse(value, out int id) ? id : 0;
            }
        }

        public string UserName =>
            User.Identity?.Name ?? "";

        public string Role =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? "";

        public int OrganizationId
        {
            get
            {
                var commonInfo = _commonRepository.GetCommonInfo(UserId);
                return commonInfo.OrganizationId;
            }
        }
    }
}
