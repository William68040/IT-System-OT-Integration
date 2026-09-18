# Jewelry Factory – IT/OT Integration

## Viktiga instruktioner

### Databas

Databasen heter JewelryFactoryDb och körs på:

(localdb)\MSSQLLocalDB

### Uppdatera databasen

Kör kommandot i Package manager console:

Update-Database

## Admin-login

Username: admin

Password: Admin123

Lösenordet sparas som en BCrypt-hash i databasen.

### Program

Projektet består av tre Console Applications:

### 1. OrderManagement – Systemet för att skapa, visa och hantera produktionsordrar.

### 2. IntegrationService – Systemet som hämtar ordrar och skickar dem vidare till rätt produktionsmaskin.
   
### 3. ProductionSystem – Systemet som simulerar produktionen och hanterar produktionsjobben.
