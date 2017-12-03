# Homework 3 RAUPJC
- Vrijeme roka se pokazuje pokraj naziva (ono se prikazuje u danima, satima ili minutama, ovisno o količini vremena preostalog)
- Labele se stvaraju automatski (pri dodavanju novog todo-a navedemo labele odvojene zarezom, te onda će se za svaku od njih pokušati pronaći već postojeća sa istim imenom, a ako je nema, onda se stvara nova)
- Labele ne ovise o case-u (drugim riječima, ne razlikuju se lowercase i uppercase slova)
- Labelama je dodijeljena boja prema imenu (nikakav podatak vezan za boju se ne sprema u bazu)
- AddViewModel, CompletedViewModel i IndexViewModel su redundantni (dodao sam ih naknadno samo zato jer je to zahtijevao zadatak), dakle može ih se zamijeniti elementima TodoViewModel to jest IEnumerable<TodoViewModel>
- 'Todos' u izborniku će biti prikazan samo za logirane korisnike
