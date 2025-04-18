# N2.Core.Abstractions

This repository provides the abstractions for generic core utilities for .NET-based projects.

## N2.Core

The `N2.Core` namespace provides a set of models and utilities for handling responses, 
data structures, and other core abstractions in a .NET-based application.

### Components

- **IResult<T>**: Represents the result of an operation, including properties like `Type`, `Success`, `Message`, and a generic `Value`.
- **IRequestResult<T>**: Extends `IResult<T>` to include additional properties or methods specific to request results.
- **SelectItem<T>**: Represents a selectable item with a generic value and display text.
- **SelectItemList<T>**: A collection of `SelectItem<T>` objects, providing methods for managing selectable items.
- **ValidationResult<T>**: Represents the result of a validation operation, including a generic value and validation messages.

### Services

- **IEmailService**: Interface for sending emails.
- **ISettingsService**: Interface for managing application settings.
- **ILogService**: Interface for logging application events.
- **IBackgroundWorker**: Interface for managing background tasks.
- **IAzureFunctionClient**: Interface for interacting with Azure Functions.
- **IActivityLogger**: Interface for logging user activities.
- **IActivityLoggerFactory**: Factory for creating instances of `IActivityLogger`.
- **ITextService**: Interface for text manipulation and formatting.
- **IStdOut**: Interface for standard output operations.
- **IValidation**: Interface for validating objects or data.
- **IHtmlString**: Interface for working with HTML strings.
- **IHtmlDisplayPart**: Interface for rendering parts of an HTML display.

### Utilities

- **HtmlString**: Represents an HTML string.
- **ErrorCode**: Enum defining error codes for application operations.
- **PagingInfo**: Represents pagination information for data queries.
- **SelectOption**: Represents a selectable option in a dropdown or list.
- **SelectOptionsExtensions**: Extension methods for working with `SelectOption` objects.

## N2.Core.Dms

The `N2.Core.Dms` namespace provides abstractions for handling document management systems (DMS) in a .NET-based application. 
It includes interfaces and models to simplify DMS operations.

### Components

- **IDocument**: Represents a document in the DMS, including properties like `Id`, `Name`, and `ContentType`.

## N2.Core.Http

The `N2.Core.Http` namespace provides abstractions for handling HTTP requests and responses in a .NET-based application. It includes interfaces and models to simplify HTTP communication.

### Components

- **IHttpRequest**: Represents an abstraction for an HTTP request, providing properties and methods to access request data.
- **IHttpResponse**: Represents an abstraction for an HTTP response, allowing manipulation of response data.
- **IHttpResult**: Defines the structure of an HTTP result, including status codes and response content.
- **IHttpClient**: Provides an abstraction for making HTTP requests, supporting dependency injection and testability.

### Usage

These abstractions can be used to create testable and reusable HTTP communication layers in your application. For example:

## N2.Core.Entity

This namespace contains the interfaces and classes that simplify the 
use of a database context in a .NET Core application.

### Components

- **IDbBaseModel** : This interface is used to define the base properties of a model class that is used in the database context.
- **IDbContext** : This interface is used to define the base properties of a database context class.
- **IBasicListModel** : This interface is used to define the base properties of a model class that is used in the database context as a list of items.
- **IChangeLog** : This interface is used to define the base properties of a model class that is used in the database context as a change log.
- **IConnectionStringService** : Helper service to find a connection string based on the environment or user context.
- **IModifiable** : This interface is used to define the base properties of a model class that is used in the database context as a modifiable item.
- **IDesignComponent** : This interface is used to define the base functionality for a component that is used to manage a specific data model.
- **ICoreDataContext** : This interface is used to define the base properties of a database context class that is used to manage the core data models.

## N2.Core.Identity

The `N2.Core.Identity` namespace provides abstractions and models for managing user authentication, roles, and access control in a .NET-based application.

### Components

- **IAuthenticator**: Defines methods for authenticating users and managing authentication tokens.
- **IIdentityUser**: Represents a user in the identity system, including properties like username, email, and roles.
- **IIdentityRole**: Represents a role in the identity system, including associated permissions.
- **IUserManager**: Provides methods for managing users, such as creating, updating, and deleting user accounts.
- **IUserContext**: Represents the current user's context, including identity and access information.
- **IUserContextFactory**: Factory for creating instances of `IUserContext`.
- **IUserAccess**: Defines methods for checking user permissions and access rights.
- **RoleRight**: Represents the relationship between roles and their associated rights.
- **SystemRoles**: Provides predefined system roles for common use cases.
- **UserLogin**: Represents a user's login information, such as login provider and key.
- **UserAlert**: Represents alerts or notifications for a user.
- **Priority**: Enum defining priority levels for user-related operations or alerts.

### Usage

These abstractions can be used to implement authentication and authorization in your application.
For example, you can create a custom user manager by implementing the `IUserManager` interface 
and use the `IAuthenticator` to handle user login and token generation.

## N2.Core.Models

The `N2.Core.Models` namespace provides a set of models and utilities for handling responses, data structures, and other core abstractions in a .NET-based application.

### Components

- **ResponseModel**: A base class for response models, providing properties like `Type`, `Message`, and `Success` to represent the status of an operation.
- **StringResponseModel**: A response model for operations that return a string value. It includes factory methods like `NoContent`, `Fail`, `Accept`, and `NoChange` for common response scenarios.
- **GuidResponseModel**: A response model for operations that return a GUID value.
- **AcknowledgeResponse**: Represents a response with an acknowledgment code and message.
- **ResponseLevel**: Enum defining levels of response severity, such as `Info`, `Warning`, and `Error`.
- **EditMode**: Enum representing different modes of editing, such as `Create`, `Update`, and `Delete`.
- **AcknowledgeCode**: Enum defining acknowledgment codes for operations, such as `Success`, `Failure`, and `Pending`.

