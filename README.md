# Swordhero

## Time Management
- This project was developed in about 9 hours.
- It took me about 6 and a half hours to complete the mandatory phase
  - 3 hours on the player controller (joystick controller + animations + auto target and auto attack enemies + replaying Archero for Controller reference)
  - 1 hour for enemies management
  - 2 hours for the weapon system (I added a UI menu to switch between weapons to make testing and comparing the different weapon easier)
  - 30 minutes for various tweaks and fixes
- I then spent the remaining time adding various signs, feedbacks, small improvements, and more tweaking
  - Feedbacks on enemies hit (camera shake, hit fx, enemy blink, damage displayed)
  - Enemies Life gauge
  - Critical hits
  - Replacing floor texture for better contrast
## Difficulties
The main difficulty I encountered was wrapping my head around the idea of auto attacking while dealing melee damage (instead of high range damage like in Archero).
<br>
When I started the project, I used a detection range, to determine whether the player had a target, and an attack range
for the player to actually attack. This resulted in a very idle game : you got close to a group
of enemies, released the joystick, and just watched your character attack.
<br>
I didn't think this was very satisfying, so I decided to actually remove to detection range, and use only the attach range
to determine if the player has a target.
<br>
I think the result is much better this way, but it means I lost some time developing the behavior for the player when they have to go
to a target that is in detection range but not in attack range.
<br> However, the game could work in a more "passive" way, where you could watch your character defeat enemies and only choose to activate skills at some time or chose upgrades when leveling up (like in [Nonstop Knight](https://play.google.com/store/apps/details?id=com.koplagames.kopla01&hl=fr&gl=US)
## Could be done differently
## Improvement ideas
#### Gameplay
- I would have liked to add behaviors for the enemies, to add a dodge phase, dividing the gameplay between attacking and dodging.
- I would have also liked adding some obstacles. I had thought about that from the beginning, which is why I added the NavMesh package to the project.
- I think it would add a lot of dynamism to the game to add different attack animations, that we could play when attacking the same enemy to give a combo effect.
- With the boundaries that I setup for the enemies spawn, they often spawn off screen. I think a UI interface with arrows indicating the direction of the off screen enemies would be a very nice add on.
#### Tech
- There is a global pooling system and another pooling system specific to the enemies, ideally I would have merged those
- Since there is not a lot of UI, I kept it fairly simple, but it would be nicer to have a UI Manager system handling the UI links.
## Comments
The game was really fun to develop, thank you for providing the base project setup.