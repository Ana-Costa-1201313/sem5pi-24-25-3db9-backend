# US 5.1.12

## 1. Context

This task appears in the start of the project's development, to be able to create a staff profile.


## 2. Requirements

**US 5.1.12** As an Admin, I want to create a new staff profile, so that I can add them to the hospital's roster.

**Acceptance Criteria:**

- Admins can input staff details such as first name, last name, contact information, and specialization.
- A unique staff ID (License Number) is generated upon profile creation.
- The system ensures that the staff's email and phone number are unique.
- The profile is stored securely, and access is based on role-based permissions

**Dependencies/References:**

It is also required that the user is registered and logged in as an admin.


## 3. Analysis

For this US were considered the requirements specified in the project's description and the client's answers. 
Some relevant answers excerpts are here specified:

- *"When it comes to patients and healthcare staff ... So the email is the identifying attribute, right? For users, or is it the username? It's the username, okay? But typically, as you know, nowadays, most of the usernames that you have in all the systems are your email, okay? So they hack, kind of, instead of you allowing to create a specific username, you use your own email as the username ... you should use the email as the username."*

- **Question:** How should the specialization be assigned to a staff? Should the admin write it like a first name? Or should the admin select the specialization?
  - **Answer:** The system has a list of specializations. staff is assigned a specialization from that list.


- **Question:** Boa tarde, gostaria de saber se é objetivo o sistema diferenciar as especializações para cada tipo de staff. Ou seja se temos de validar que a especialização x só pode ser atribuída por exemplo a um membro do staff que seja doctor, ou se consideramos que qualquer especialização existente no sistema pode ser atribuída a qualquer staff ficando da autoria do responsável por criar os perfis atribuir especializações válidas de acordo com a role do staff.
  - **Answer:** As especializações são independentes do professional ser médico ou enfermeiro


- **Question:** -Médicos e enfermeiros podem ter apenas uma especialidade ou podem ser especialistas em várias? -Quem faz parte do staff? Toda a gente na sala de operação? Se sim, todos eles tem as suas respetivas especialidades, incluindo técnicos? 
  - **Answer:** Um médico ou enfermeiro apenas tem uma especialização. no staff apenas consideramos médicos e enfermeiros


- **Question:** Regarding the specializations, do doctors, nurses, and technicians share the same group of specializations, or does each type of professional have distinct, role-specific specializations? Could you clarify how these specializations are categorized?
  - **Answer:** They share the same set of specializations.


- **Question:**  How are duplicate patient profiles handled when registered by both the patient and admin?
  - **Answer:** The system checks the email for uniqueness. The admin must first create the patient record, and then the patient can register using the same email.

"When we are registering users to the platform, do we immediately need to give them a profile? Like when we register a user, do we immediately need to create their patient or healthcare professional staff profile? Or can we just leave them as a user to log in? Because there's some user stories that reference the creation of users with roles, but not necessarily their profile. I think that's up to you, honestly.
It will boil down to a design decision. From the functional perspective, it's not something important. Okay, so I think it will be more a question of does it make more sense for you from the technical perspective to do it immediately or do it afterwards?"

- **Question:** Are healthcare staff IDs unique across roles?
  - **Answer:** Yes, staff IDs are unique and not role-specific (e.g., a doctor and nurse can share the same ID format).

"... regarding healthcare staff, we want to understand if the staff ID is unique or if it is, for example, if it is unique in the sense that, for example, doctor is 1, nurse is 2, or if, for example, the doctor has ID 1, there is a nurse ID 1? Okay, I understand what the issue is. Employees are identified by a mechanical number, basically. And it doesn't matter if this typing number is for a doctor, a nurse, or an assistant. It's a number of employees ..."


The following **HTTP requests** will be implemented:
- POST (to register the new staff profile)

## 4. Design

This section presents the design adopted to solve the requirement.

### 4.1. Level 1 Sequence Diagram

This diagram guides the realization of the functionality, for level 1 procecss view.

![US5.1.12 N1 SD](US5.1.12%20N1%20SD.svg)


### 4.2. Level 2 Sequence Diagram

This diagram guides the realization of the functionality, for level 2 procecss view.

![US5.1.12 N2 SD](US5.1.12%20N2%20SD.svg)


### 4.3. Level 3 Sequence Diagram

This diagram guides the realization of the functionality, for level 3 process view.

![US5.1.12 N3 SD](US5.1.12%20N3%20SD.svg)


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
- The staff member only has one specialization.


## 5. Implementation

The implementation of this US is according to the design, as can be seen in the diagrams presented before.

All commits referred the corresponding issue in GitHub, using the #15 tag, as well as a relevant commit message.


## 6. Integration/Demonstration

To register a new staff profile, run the Backoffice app and send a POST HTTP request with the new staff data.

## 7. Observations

This work was guided by the project provided in ARQSI classes.