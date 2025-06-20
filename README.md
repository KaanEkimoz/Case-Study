# Case Study

SETUP: Download "WINDOWS BUILD.ZIP" file in repo and extract it to a folder then run the exe.

Note: The lock-on system currently targets the dummy in the direction the player is facing, rather than the closest enemy. I chose this approach intentionally for better game feel, as locking onto an unseen enemy nearby felt like a poor design choice. Prioritizing what the player is actually looking at creates a more intuitive and satisfying experience.

Controls
WASD Keys for movement
TAB Key to Lock On Target

# Step-by-Step: What I Did

-I created a structured implementation plan, closely following the order described in the assignment document.

-I started with a clean Unity project and imported the necessary assets (Cinemachine, DoTween, and my own helper scripts).

-I completed the third-person movement and rotation controls with smooth acceleration and damping.

-Before setting up the camera, I explored the Cinemachine documentation and sample scenes to identify useful components. I configured two separate cameras: one for lock-on mode and one for free-look mode.

-For movement and strafing animations, I used Mixamo animations. Using blend trees and animation layers, I implemented smooth transitions and directional movement.

-After finalizing movement and camera systems, I created a basic 3-step combo attack system and synchronized it with the player's movement and input.

-I implemented a simple combat and health system for the enemy dummy using interfaces and hitbox triggers.
-I added sound effects and visual effects to enhance feedback. Since the project is small and focused on game feel, I mostly used UnityEvents to trigger them cleanly.

-Lastly, I spent extra time refining the combat system and improving the overall game feel through camera shakes, hit reactions, and animation timing.

# Faced Challenges and Solutions

-First of all, Mixamo was down for more than a day during the case study period, so initially I couldn't find the necessary animations. As a result, I searched through many projects to find and implement useful animations.

-In the first version of the combat system, due to a lack of animations, I tried using avatar masks to separate body parts. The idea was for the upper body to perform attack animations while the lower body (legs) continued normal movement animations simultaneously. However, during tests, this didn’t feel natural, so I decided to change the approach. Fortunately, after Adobe fixed Mixamo, I rebuilt the animation system using better animations.

-For sword damage detection, I was unsure whether to use trigger colliders or raycasts. After researching and testing, I found that trigger-based detection was the best solution.

-For strafing movement, I heavily used animation layers and blends, but since the underlying logic was simple, I was able to implement it quickly.

-Due to limited time, I couldn’t focus enough on refactoring my code and organizing the folders, but I kept everything as clean and structured as possible. Instead, I prioritized polishing the game feel and completing a functional build.

