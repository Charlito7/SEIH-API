using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Enums;

public enum UserRoleEnums
{   User,
    Seller,
    Manager,
    Admin,
    SuperAdmin,
    Associate,
    SEIHMANAGER,
}
/* 
 ITAdmin: all read/write privileges
 Manager: all read 
 Admin: create stock/ some read write privilege
 Seller: Sales privileges
 User
 Planner

 */
