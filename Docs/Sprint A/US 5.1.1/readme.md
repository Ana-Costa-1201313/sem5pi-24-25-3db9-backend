# US 1010

## 1. Context

This task appears in the start of the project's development, to be able to register new backoffice users.


## 2. Requirements

**US 5.1.1** As an Admin, I want to register new backoffice users (e.g., doctors, nurses, technicians, admins) via an out-of-band process, so that they can access the backoffice system with appropriate permissions.

**Acceptance Criteria:**

- Backoffice users (e.g., doctors, nurses, technicians) are registered by an Admin via an internal process, not via self-registration.
- Admin assigns roles (e.g., Doctor, Nurse, Technician) during the registration process.
- Registered users receive a one-time setup link via email to set their password and activate their account.
- The system enforces strong password requirements for security.
- A confirmation email is sent to verify the user’s registration.


**Dependencies/References:**

It is also required that the user is registered and logged in as an admin.


## 3. Analysis

For this US were considered the requirements specified in the project's description and the client's answers. 
Some relevant answers excerpts are here specified:

- *"When it comes to patients and healthcare staff ... So the email is the identifying attribute, right? For users, or is it the username? It's the username, okay? But typically, as you know, nowadays, most of the usernames that you have in all the systems are your email, okay? So they hack, kind of, instead of you allowing to create a specific username, you use your own email as the username ... you should use the email as the username."*


The following **HTTP requests** will be implemented:
- POST (to register the new user)
- GET (to check the new users)
- PATCH (updating the password and active the account)


## 4. Design

This section presents the design adopted to solve the requirement.

### 4.1. System Sequence Diagram

This diagram presents the interaction between the user and the application, when executing this functionality.


### 4.2. Sequence Diagram

This diagram guides the realization of the functionality.


### 4.3. Class Diagram

This diagram presents the classes that support the functionality.


### 4.4. Applied Design Patterns


### 4.5. Tests


## 5. Implementation

The implementation of this US is according to the design, as can be seen in the SD and CD presented before.

All commits referred the corresponding issue in GitHub, using the #1 tag, as well as a relevant commit message.


## 6. Integration/Demonstration



## 7. Observations

This work was guided by the project provided in ARQSI classes.