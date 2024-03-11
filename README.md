# Jwt Authentication


This project is designed to outline an example of a JWT auth server within ASP.NET. 
</br></br>

## AuthController 

Used to generate authentication with the server via user details, in this system we just use in the form of a post request;
- Username
- Password
</br></br>

## AuthenticationService

AuthenticationService is the core domain logic which generates a token based on the authentication information provided by AuthController.

This code is creating a new instance of the ClaimsIdentity class, which is a part of the System.Security.Claims namespace in .NET. ClaimsIdentity represents an individual participant, or an identity, with a collection of claims.

A claim is a statement about an entity (typically, the user) and, optionally, additional metadata about this statement. It's a way of representing identity information in a standardized way across the application.
```
Subject = new ClaimsIdentity(new[] 
{ 
    // map new claims from object here
    // example 
    // new Claim("id", user.Id.ToString()),
})
```
We sign the token with an expiration date, the user can generate a new token if required by calling the AuthController endpoint again.
```
Expires = DateTime.UtcNow.AddDays(7),
```
</br>

A `key` used in the creditial signing is retrieving the value of the `Secret` setting from the `AppSettings` section of the application's configuration, and then converting that value into a byte array.

```
var key = Encoding.ASCII.GetBytes(
    configuration.GetSection("AppSettings")
        .GetChildren()
        .First(x => x.Key == "Secret")
        .Value
); 
```
</br>

This line of code is creating a new instance of the SigningCredentials class, which is a part of the System.IdentityModel.Tokens namespace in .NET. SigningCredentials are used to digitally sign a security token. This digital signature provides a way to ensure the integrity and authenticity of the token.

In summary, this line of code is creating a SigningCredentials object that will use a symmetric key and the HMAC-SHA256 algorithm to digitally sign a security token.
```
SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
```
</br>

## JwtMiddleware

The core middleware in this system used when a request is made to authenticate a token against.  

This code is creating a new anonymous object and assigning it to the "User" key in the context.Items dictionary. The context.Items dictionary is often used in .NET to store per-request data that needs to be accessible throughout the entire request's lifecycle.

The anonymous object is intended to have properties that are mapped from the JwtToken.Claims collection. A JWT (JSON Web Token) is a compact, URL-safe means of representing claims to be transferred between two parties. The claims in a JWT are encoded as a JSON object that is used as the payload of a JSON Web Signature (JWS) structure.
```
context.Items["User"] = new
{
    // map new properties from JwtToken.Claims 
    // example
    // Id = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value),
};
```
</br>

## Setup

```
app.UseMiddleware<JwtMiddleware>();
``` 
This line is adding a custom middleware component, JwtMiddleware, to the middleware pipeline. The JwtMiddleware is a class that you have defined in your application typically `Startup` or `Program`.
</br></br>

# How to auth based on roles?

Within `AuthenticationService` we need to ensure we add roles to the claims when signing the token with the following
```
new Claim("roles", string.Join(',', roles.Where(x => user.Roles.Any(r => r == x.Id)).Select(r => r.Name)))
```
</br>

Following adding the claims to the token we then need to extract them within `JwtMiddleware` when descphering the token metadata with
```
Roles = jwtToken.Claims.First(x => x.Type == "roles").Value.Split(',')
    .Select(role => new Role { Name = role })
```