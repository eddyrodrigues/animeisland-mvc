using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AnimeIsland.App.Conf;

public class PermissaoAcessoAttribute : TypeFilterAttribute
{
    public PermissaoAcessoAttribute(string claimType, string claimValue) : base(typeof(PermiteAcessoFilter))
    {
        Arguments = new object[] { new Claim(claimType, claimValue) };
    }
}

public class PermiteAcessoFilter : IAuthorizationFilter
{
    readonly Claim _claim;

    public PermiteAcessoFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "account",
                action = "login",
                ReturnUrl = context.HttpContext.Request.Path.ToString()
            }));
            //new RedirectToActionResult("LoginAsync", "Identity", new RouteValueDictionary( new { ReturnUrl = context.HttpContext.Request.Path.ToString() }));
            //context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { page = "/account/login",));
            return;
        }

        var claimsValues = _claim.Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
        var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && claimsValues.Any(uc => c.Value.Contains(uc)));
        if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
        {
            context.Result = new ForbidResult();
        }


    }
}

public class CustomAuthorization
{
    public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
    {
        var hasClaim = context.User.Claims.Any(c => c.Type == claimName && claimValue.Any(uc => c.Value.Contains(uc)));
        return context.User.Identity.IsAuthenticated &&
            hasClaim;
               //context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
    }

}