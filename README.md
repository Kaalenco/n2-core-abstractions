# n2-core

Basic functionality for any project. It contains abstractions and utilities that 
are commonly used across different projects. This package was created after realizing
that many projects were using similar abstractions and utilities, leading to 
code duplication and maintenance challenges. It aims to provide a centralized
set of abstractions and utilities that can be reused across projects, reducing 
duplication and improving maintainability.

# Change Log

- **2024-03-29** Version 1.0.1 : Collected several abstractions and utilities into a single package.
	- Initial release of N2.Core with basic abstractions and utilities.
- **2025-04-15** Version 1.2.1 :  Isolating abstractions into separate namespaces for better organization.
	- Introduced `N2.Core.Abstractions` namespace for HTTP-related abstractions.
	- Introduced `N2.Core.Entity` namespace for database context and model abstractions.
	- Introduced `N2.Core.Identity` namespace for user authentication and role management abstractions.
	- Introduced `N2.Core.Models` namespace for response models and utilities.
	- Introduced `N2.Core.Dms` namespace for document management services abstractions.
	- Added initial documentation for each namespace and its components.
- **2025-05-28** Version 1.3.0 : Breaking changes due to namespace restructuring.
	- Added a unit test project with some initial tests.
	- Rename `N2.Core.Models` to `N2.Core.Commands` and extended command handler abstractions and base classes for command handling.
	- Extended Entity namespace with additional interfaces for database context management.
	- Extended Identity namespace with additional user and role management interfaces.