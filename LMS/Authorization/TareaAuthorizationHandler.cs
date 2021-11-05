using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Authorization
{
    public class TareaAuthorizationHandler :AuthorizationHandler<TareaAuthorization, Tareas>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                      TareaAuthorization requirement,
                                                        Tareas resource)
        {
            var Iduser = resource.Prof.UserId;

            if (context.User.HasClaim(ClaimTypes.NameIdentifier, Iduser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }



    }
}
