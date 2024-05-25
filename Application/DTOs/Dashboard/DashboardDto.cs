using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketoo.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int? UsersCount { get; set; }
        public int? ProductsCount { get; set; }
        public int? CategoriesCount { get; set; }
        public int? RolesCount { get; set; }
    }
}
