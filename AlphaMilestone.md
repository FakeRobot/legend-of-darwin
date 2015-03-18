# To Do #

  * Improve level design on level 1-3
  * Slow human-darwin down
  * Slow zombie-darwin down even more
  * Cue: what state is the zombies in?
    * add noise when zombie sees darwin
    * ~~add exclamation mark when zombie sees darwin~~
    * ~~add question mark when zombies do not see darwin anymore~~
  * Speed up default zombie
  * ~~Make the screen and grid size bigger!!~~
  * fix bug: zombies kills you when you're not beside it...
  * ~~Possibly improve controls (get rid of spacebar for exiting)~~

# Level 4: Conga Line #

If the any zombie see's you as a human, you dead.
When you're a zombie:
  * If you're on the dance floor, head zombie gets mad and kills you after 3 seconds
  * If you're in the conga line, you're good.
  * If you're in the conga line and you're too slow, head zombie kills you.
  * Two switches, which each release caged zombies.
  * Can only be human to turn switch on, so timing is key (can only turn on switch when the gap in the zombie line passes you)
  * Each switch also opens up part of the door to the stairs.
  * Conga leader zombie. Travels in patrol path around dance floor
  * Follower zombies

# Level 5: Flame Thrower #

  * Four flame throwing zombies
    * Around perimeter of level
    * Can throw flames up to 3 grid squares
    * Will kill darwin in human or zombie form
  * Snake zombies:
    * Charge darwin at medium speed when on the same X or Y axis as them
    * They want to push darwin towards the flame thrower
  * Block puzzle
    * Need to arrange blocks in the level to open the door

  * You get easily killed in this level
  * If this level is too easy, add random goo spots
  * If darwin is a zombie, then the snakes don't attack you. Only implement this if there is no potion.

# Level 6: Final Boss #

  * Boss zombie
    * Four grid size
    * Moves every little, a couple squares back and forth
    * Boss wil spit out potions and goo
    * boss will have 3 states:
      1. walking
      1. chucking
      1. gape
    * he will only eat baby zombies when he is gapeing
    * cannot damage darwin as a zombie
    * after eating 4 babies zombie, he dies

  * Baby Zombies
    * Two cribs, where the baby zombies keep coming out of
    * Standard amount of life -  when the end of their life gets close, they blink, then EXPLODE!
    * Explosions take up every square around the baby
    * these are CRAZY ZOMBIE BABIES
    * baby zombies follow darwin all the time, zombie or human

  * Need to lead the baby zombies to the boss so he will eat them
    * need to be a zombie near the boss, or else the boss will kill you
    * if a baby zombie explodes near the boss, the boss is not damaged.