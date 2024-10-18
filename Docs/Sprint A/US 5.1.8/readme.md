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


- **Question:** 
  - **Answer:** 


- **Question:** 
  - **Answer:** 


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