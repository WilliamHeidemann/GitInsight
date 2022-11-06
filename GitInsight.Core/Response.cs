namespace GitInsight.Core;

public enum Response {
    NotFound, 
    Found, 
    Updated, 
    Created, 
    Conflict,
    BadRequest,
    NoContent
}