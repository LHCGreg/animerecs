The AnimeRecs.RecService protocol is a TCP protocol. The client connects to the server, then sends a UTF-8 encoded JSON Operation object, then closes the send half of the connection. An Operation object is:

{
	OpName: "string",
	
	(optional)
	Payload: 
	{
		...
	}
}

See ConnectionServicer.cs for valid op names, the kind of input they take, and the kind of output they give.

Upon receiving an operation from a client, the server does what it needs to do, returns a UTF-8 encoded JSON Response object, and closes the connection. A Response object is:

{
	(null if no error)
	Error:
	{
		ErrorCode: "string",
		Message: "string"
	},

	(optional)
	Body:
	{
		...
	}
}

The ErrorCode is what type of error occured. See ErrorCodes.cs in AnimeRecs.RecService.DTO for possible error codes. The ErrorCode can be used to programatically handle certain kinds of errors. The Message is an error message that may be helpful. The contents of Body, if present, are dependent on the operation.