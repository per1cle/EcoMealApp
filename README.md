# Cerinte si Specificatii EcoMeal

## 1. Descriere Generala
EcoMeal este o aplicatie web destinata combaterii risipei alimentare. Aceasta conecteaza restaurantele, brutariile si magazinele alimentare cu clientii, permitand vanzarea pachetelor de mancare ramasa la finalul zilei la preturi reduse.

## 2. Roluri Utilizatori
- Client (Customer)
- Proprietar Afacere (Business Owner)
- Administrator (Admin)

## 3. Cerinte Functionale

### Autentificare si Conturi
- Inregistrare si autentificare securizata pe baza de token JWT.
- Posibilitatea de inregistrare ca simplu client sau ca proprietar de afacere.
- Delimitarea drepturilor pe baza rolurilor (Admin, Business, Customer).

### Gestionarea Afacerilor si Aprobarea lor
- Proprietarii de afaceri isi pot crea si edita profilul afacerii (nume, adresa, descriere, imagine, tip afacere, coordonate GPS - latitudine si longitudine).
- Cand o afacere este creata, aceasta intra automat in starea de asteptare a aprobarii (IsApproved = false).
- O afacere neaprobata nu este vizibila in magazinul public (Shop) si nu isi poate vinde pachetele pana cand nu este aprobata.
- Administratorul poate vedea toate afacerile si are optiunea de a aproba sau revoca oricand o afacere.

### Gestionarea Pachetelor
- Proprietarii de afaceri si administratorii pot adauga, edita si sterge pachete alimentare.
- Fiecare pachet contine: nume, descriere, tip pachet, pret, cantitate (stoc disponibil), interval orar de ridicare (de la / pana la) si imagine.
- Stocul scade automat la plasarea comenzilor.
- La anularea unei comenzi, stocul se reface automat.

### Pagina de Shop si Harta Interactiva
- Clientii pot vizualiza magazinele partenere aprobate fie sub forma de lista (carduri), fie pe o harta interactiva (Leaflet.js / OpenStreetMap).
- Pe harta interactiva, fiecare afacere are un pin pe baza coordonatelor setate.
- La click pe un pin de pe harta, apare un popup cu detalii si butonul de vizualizare pachete.
- Pachetele unui magazin se deschid intr-un meniu modal pentru a putea fi adaugate in cos.

### Logica Cosului si a Comenzilor
- O comanda poate contine pachete de la un singur restaurant.
- Daca un client incearca sa adauge in cos produse de la un al doilea restaurant, adaugarea este blocata si clientul este avertizat sa goleasca cosul curent daca doreste sa schimbe restaurantul.
- Validarea unicitatii restaurantului pe comanda este verificata atat in interfata (frontend), cat si in logica de backend.

### Statusurile Comenzilor si Istoric
- Fiecare comanda trece prin statusurile: Pending -> Confirmed -> Completed sau Cancelled.
- Clientul poate vedea istoricul comenzilor sale active si trecute in pagina "My Orders".
- Clientul poate anula o comanda cat timp este in starea Pending sau Confirmed.
- Comerciantul poate vedea comenzile primite in dashboard-ul afacerii si le poate schimba statusul (Confirm, Mark Completed, Reject/Cancel).

### Serviciul de Notificari prin Email
- Trimitere automata de emailuri tranzactionale catre client in functie de stadiul comenzii:
  - Email de confirmare a comenzii (Confirmed) cu sumarul produselor, totalul de plata, adresa si link catre Google Maps pentru locatia restaurantului.
  - Email de finalizare a comenzii (Completed) dupa ce pachetul a fost ridicat.
  - Email de anulare a comenzii (Cancelled) in cazul in care comanda a fost respinsa sau anulata.

### Panou de Administrare (Admin)
- Administratorul are pagini dedicate in meniu pentru:
  - Businesses: vizualizare, adaugare, editare, stergere si aprobare/revocare afaceri.
  - Packages: vizualizare, adaugare, editare si stergere pachete.
  - Orders: vizualizare toate comenzile din aplicatie, filtrare dupa magazin si status, schimbare status si stergere.
  - Users: vizualizare toti utilizatorii, filtrare dupa rol, adaugare, editare si stergere utilizatori.
  - Administration: gestionare tipuri de afaceri (BusinessType) si tipuri de pachete (PackageType).

## 4. Tehnologii Utilizate
- Frontend: Blazor WebAssembly (.NET 10), Bootstrap 5, Leaflet.js, JavaScript Interop.
- Backend: ASP.NET Core Web API (.NET 10).
- Baza de date: Microsoft SQL Server, Entity Framework Core 10 (Code-First).
- Securitate: ASP.NET Core Identity, JWT Bearer Authentication.
- Serviciu Email: System.Net.Mail (SMTP).
