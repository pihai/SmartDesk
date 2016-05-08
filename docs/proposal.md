# Projekt SmartDesk - Proposal

Heutzutage zeichnen schon viele Menschen ihr Bewegungsprofil mit einem „intelligenten“ Armband auf. Ihr Ziel ist eine bewusstere und gesündere Lebensweise. Doch was ist mit der vielen Zeit die wir fast regungslos im Büro sitzen. Hier kann ein (elektrischer) Stehtisch Abhilfe schaffen. Aber auch permanentes stehen ist auf Dauer ungesund. Darum möchten wir ein Gerät entwickeln, dass die Steh- und Sitzzeiten aufzeichnet und auf dieser Basis dem Anwender Empfehlungen und Auswertungen anbieten kann. Nachfolgend die geplanten Teilbereiche dieses Projekts.

## Must-Haves

- Entwicklung einer Arduino/Pi/Netduino Anwendung die über einen Abstandssensor erkennt ob der Anwender sitzt oder steht
-	Verbindung zum PC, sodass die Software erkennen kann ob der Anwender aktiv arbeitet oder sich nicht am Arbeitsplatz befindet
-	Übermitteln der Daten in die Cloud (z.B. Microsoft IoT-Hub)
-	Speichern, aufbereiten und aggregieren der Daten für spätere Auswertungen
-	Visualisierung der Daten in Form einer Web-Seite (Tages-Verlauf, Aktivitäts- und Inaktivitätszeiten, Wochenverlauf, ...)
-	Senden einer Benachrichtigung, wenn der Anwender zu lange sitzt (z.B. durch Browser-Notification)

## Nice-To-Haves

Je nach Aufwand der Must-Have Arbeitspakete wären auch noch folgende Funktionen interessant:

-	Benachrichtigung durch einen Chat-Bot (z.B. über Skype, ...)
-	Es wäre auch interessant den Chat-Bot so intelligent zu gestalten, dass er verschiedene Fragen beantworten kann, wie z.B.:
  -	Wie lange bin ich heute gestanden?
  -	Soll ich lieber stehen oder sitzen?
  -	Wie lange sitze ich bereits?
  -	Wie lange bin ich letzte Woche gestanden?
  -	...
-	Interessant wäre auch eine Steuerung des Tisches, z.B. über einen Sprachbefehl. Oder eine automatische Positionsveränderung, wenn der Anwender eine Pause macht. (Kann natürlich auch gefährlich/unerwünscht sein)
