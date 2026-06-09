# 🗼 Magical Tower

> A wave-based tower defense game built in Unity where a magical tower defends itself against waves of enemies using spells.

![Gameplay](docs/gameplay.gif)

---

## 🎮 Gameplay

- Enemies spawn outside the screen and move toward the tower
- The tower automatically casts spells at enemies in range
- Survive all waves to win; tower HP drops to 0 — game over

---

## ✨ Spells

| Spell | Description |
|---|---|
| **Fireball** | Launches a projectile that explodes on impact, dealing AoE damage and applying burn |
| **Barrage** | Fires homing arc projectiles at every enemy in range simultaneously |

---

## 🏗️ Architecture

```
Assets/_Project/Scripts/
├── Core/           # EventBus, GameStateService, ObjectPool, GameEvents
├── Data/           # ScriptableObject configs — WaveConfig, EnemyConfig
├── Domain/
│   ├── Enemy/      # Enemy MonoBehaviour, EnemyFactory, Behaviours (Movement/Attack/Death)
│   ├── Spells/     # SpellBehaviourBase, ISpellCommand, Projectiles
│   ├── Tower/      # Tower, TowerHealth
│   └── Effects/    # StatusEffect, BurnEffect, FlashEffect
├── Systems/        # CombatSystem, SpawnSystem, DamageSystem
├── UI/             # UIManager, Presenters, DamageNumbers
└── Infrastructure/ # GameLifetimeScope (VContainer DI), DebugController
```

**Key patterns:**
- **EventBus** — decoupled communication between systems
- **VContainer** — dependency injection, no singletons
- **ObjectPool** — enemies and projectiles are pooled
- **Command Pattern** — each spell is an `ISpellCommand`
- **Strategy Pattern** — enemy behaviours are ScriptableObjects

---

## ⚙️ Adding a New Enemy

1. Create `EnemyConfig` via `Assets → Create → MagicalTower → Configs → Enemy`
2. Assign `Movement`, `Attack`, `Death` behaviour ScriptableObjects
3. Add the config to a `WavePeriod` in `WaveConfig`

## ⚙️ Adding a New Spell

1. Create a new `SpellBehaviourBase` ScriptableObject (see `FireballBehaviour` as example)
2. Implement `ISpellCommand` with `Execute`, `Tick`, `IsReady`
3. Add the asset to the Tower's `_spells` list in the Inspector

---

## 🐛 Debug Controls

| Key | Action |
|---|---|
| `Left Ctrl` (hold) | Slow motion — `timeScale 0.2` |
| `Left Shift` (hold) | Fast forward — `timeScale 3.0` |
| `R` | Restart scene |

---

## 🛠️ Tech Stack

- **Unity** (URP)
- **VContainer** — DI container
- **UniTask** — async utilities
- **DOTween** — animations (flash effect, damage numbers)
