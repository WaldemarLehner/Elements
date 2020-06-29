
# Project: Elements

Bei Project: Elements handelt es sich um einen 2D Infinite-Runner. Das Ziel ist es von Raum zu Raum gehen und alle Gegner zu besiegen. Nach dem Bestehen eines Raumes darf der Spieler sich ein Power-Up auswählen.

![Screenshot](https://i.imgur.com/jzWjCHu.png)

# Steuerung

Gespielt wird mit Maus und Tastatur.

Mit der Maus wird die Schussposition angezeigt, mit Links-Klick wird geschossen.

Mit WASD wird der Spieler bewegt,

Mit Shift wird gesprintet.

Mit Space wird ein Dash ausgeführt.

Mit F wird interagiert.

# Projektaufbau

Das Projekt ist nach dem typischen MVC-Pattern aufgebaut.

## Model

Im Model sind alle Definitionen, sowie die Logik aller Spielelemente aufzufinden.
Eine wichtige Komponente ist dabei die Szene. Diese wird vom View aufgerufen.

## View

Im View wird der Renderer implementiert. Wird verwenden OpenToolkit, ein C#-Wrapper um openGL. Die View beinhaltet zudem Hilfsfunktionen wie bspw. Berechnungen zur Transformation von Welt-Koordinaten in Normalisierte Koordinaten.

## Controller

Der Controller beinhaltet Logik zum Erfassen von Maus und Tastatur. Der Controller ruft das Model und die View auf.
