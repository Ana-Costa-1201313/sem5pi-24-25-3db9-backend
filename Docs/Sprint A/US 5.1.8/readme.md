# US 5.1.8

## 1. Context

This task appears in the start of the project's development, to be able to create a patient profile.


## 2. Requirements

**US 5.1.8** As an Admin, I want to create a new patient profile, so that I can register their personal details and medical history.

**Acceptance Criteria:**

- Admins can input patient details such as first name, last name, date of birth, contact
information, and medical history.
- A unique patient ID (Medical Record Number) is generated upon profile creation.
- The system validates that the patient’s email and phone number are unique.
- The profile is stored securely in the system, and access is governed by role-based permissions.

**Dependencies/References:**

It is also required that the user is registered and logged in as an admin.


## 3. Analysis

For this US were considered the requirements specified in the project's description and the client's answers. 
Some relevant answers excerpts are here specified:

- **

- **Question:** It is specified that the admin can input some of the patient's information (name, date of birth, contact information, and medical history).

Do they also input the omitted information (gender, emergency contact and allergies/medical condition)?
Additionally, does the medical history that the admin inputs refer to the patient's medical record, or is it referring to the appointment history?
  - **Answer:** the admin can not input medical history nor allergies. they can however input gender and emergency contact


- **Question:** When an Admin creates a patient profile, should he already register them in the system, as users that can login, or should the registration always be responsibility of the patient?
If the latter is intended, should the patient's user credentials be linked to the existing profile?
  - **Answer:** this was already clarified in a previous meeting.

registering a patient record is a separate action from the patient self-registering as a user


- **Question:** That's right, it was clarified in a previous meeting, but I'm still not 100% clarified about my question.
I understand that the Admin can create the Pacient profile and leave the User as inactive, but how does the activation happen? If that pacient eventualy wants to register himself, should there be an option to activate an existing profile? For example, associate the e-mail from registration input with the existing profile's e-mail?
The feature 5.1.3 asks for the full registration, but doesn't say anything about profiles that already exist
  - **Answer:** the admin register the patient (this does not create a user for that patient)
optionally, the patient self-registers in the system by providing the same email that is currently recorded in their patient record and the system associates the user and the patient
there is no option for someone who is not a patient of the system to register as a user

hope this is the clarification you were missing. if not, let me know.


- **Question:** 
  - **Answer:** 


- **Question:**  
  - **Answer:** 



- **Question:** 
  - **Answer:** 



The following **HTTP requests** will be implemented:
- POST (to register the new patient profile)

## 4. Design

This section presents the design adopted to solve the requirement.

### 4.1. Level 1 Sequence Diagram

This diagram guides the realization of the functionality, for level 1 procecss view.

![US5.1.8 N1 SD](US5.1.8%20N1%20SD.svg)


### 4.2. Level 2 Sequence Diagram

This diagram guides the realization of the functionality, for level 2 procecss view.

![US5.1.8 N2 SD](US5.1.8%20N2%20SD.svg)


### 4.3. Level 3 Sequence Diagram

This diagram guides the realization of the functionality, for level 3 process view.

![US5.1.8 N3 SD](US5.1.8%20N3%20SD.svg)


### 4.4. Class Diagram

This diagram presents the classes that support the functionality.
*To do*


### 4.5. Applied Design Patterns

- **Domain Driven Development (DDD):** the focus is the business logic and not the implementation.
- **Data Transfer Object (DTO):** gives an abstraction layer to the domain, so that it's only presented specific information regarding the object.
- **Model View Controller (MVC):** allows the re-usability of components and promotes a more modular approach to the code, making it easier to manage and maintain.
- **Repository pattern:** allows access to data without sharing the details of data storing, like the database connection.
- **Service pattern:** helps keeping high cohesion and low coupling in the code by separating complex business logic from the rest of the system. They also promote reuse, as multiple parts of the system can use the same service to perform common operations.
- **Test Driven Development (TDD):** planning the tests previously to the code gives orientation lines to the development process.
- **Onion Architecture:** concentric layers structure that puts the Domin Model as the core. Promotes modularity, flexibility and testability.
- **Inversion of Control:** the responsability of object creation and dependency management belongs to a framework or external entity, so that the class doesn't need to. Promotes flexibility and decoupling.
- **Dependency Injection:** used to implement inversion of control. The dependencies are injected into a class from the outside.


### 4.6. Tests

The following tests are to be developed:
- The ID must be unique
- The email must be unique.
- The phone number must be unique.
- 


## 5. Implementation

The implementation of this US is according to the design, as can be seen in the diagrams presented before.

All commits referred the corresponding issue in GitHub, using the #9 tag, as well as a relevant commit message.


## 6. Integration/Demonstration

To register a new patient profile, run the Backoffice app and send a POST HTTP request with the new patient data.

## 7. Observations

This work was guided by the project provided in ARQSI classes.