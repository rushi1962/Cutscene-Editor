🎬 Cutscene-Editor (Unity)
=
A designer-friendly, node-based gameplay sequencing tool for Unity that allows non-programmers to author cutscenes, scripted interactions, and gameplay flows using trigger volumes and reusable sequence assets.

✨ Overview
=

Cutscene Editor is a custom Unity editor tool that lets designers define event-driven gameplay sequences without writing code.

Using a visual node editor, designers can:

- Respond to player entering a trigger volume

- Disable/enable player input

- Play sounds, animations, and Timeline cutscenes

- Insert waits, branches, and debug nodes

- Reference scene objects safely via a variable blackboard

The system separates scene logic from sequence logic, making authored sequences reusable across multiple levels.

🎯 Key Goals
=

- Enable non-programmers to author gameplay logic

- Keep sequences data-driven and reusable

- Provide a lightweight alternative to full visual scripting

- Demonstrate professional Unity tooling practices


🧩 Core Concepts
=
Sequence Asset (ScriptableObject)
-

- Stores nodes, connections, and execution order

- Scene-agnostic

- Can be reused across multiple trigger volumes

Trigger Sequence (MonoBehaviour)
-

- Lives in the scene

- Owns trigger volume and runtime context

- Provides a blackboard of variables (components, objects)

- Executes the assigned sequence when triggered

Node Graph Editor
-

- Custom EditorWindow

- Drag-and-drop nodes

- Visual connections

- Node-specific inspector fields

- Auto-saving of sequence data

🔧 Features
=
Node Types
-

- Wait Node – pauses execution for a duration

- Debug Node – logs messages during execution

- Play Sound Node – plays AudioClips

- Play Cutscene Node – plays Timeline/PlayableAssets

- Enable/Disable Player Input

Designer-Friendly Tools
-

- Visual node editor

- Context menus for node creation

- Type-safe component selection via dropdowns

- Scene references assigned without code

🛠️ Technical Highlights
=

- Custom EditorWindow and inspectors

- GUID-based node identification

- ScriptableObject serialization

- Runtime execution via coroutines

- Asset dirty tracking

- Auto-save behavior (Unity editor limitation aware)

Glimps At The Tool Inside Unity
=
![WhatsApp Image 2026-01-01 at 15 31 22](https://github.com/user-attachments/assets/c912f4c8-2def-4ab9-8b27-77b3e179a10a)


![WhatsApp Image 2026-01-01 at 15 31 14](https://github.com/user-attachments/assets/9fa378bf-edca-4a08-a2c5-01262719b0b6)


https://github.com/user-attachments/assets/6090a1ab-5619-48ba-83b4-1e16286861d9


📂 Project Structure
=
<img width="1090" height="692" alt="image" src="https://github.com/user-attachments/assets/0bb41ac9-e462-4ed5-a2f5-e1c4cdbc50fd" />

🚀 Getting Started
=

1. Clone or download the repository
2. Open the project in Unity (Unity version: 6000.2.12f1)
3. Create a SequenceAsset via Create → Sequence Asset
4. Add a TriggerSequence component to a GameObject
5. Assign the sequence asset
6. Define variables (Animator, AudioSource, etc.)
7. Click Open Sequence Editor
8. Author the sequence visually
9. Enter Play Mode and trigger the volume


🧪 Example Use Case
=

When the player enters a trigger:

1. Disable player input
2. Play a sound
3. Wait for 2 seconds
4. Play a Timeline cutscene
5. Re-enable player input

All authored without writing gameplay code.


⚠️ Known Limitations
=

- EditorWindow close button cannot be intercepted (Unity limitation)

- No nested graphs (by design)

- Single entry trigger per sequence

- Not intended as a full visual scripting replacement

📈 Future Improvements
=

- Node search window

- Reorderable variable list blackboard

- Runtime debug visualization

- Node validation and error highlighting

- Sequence preview in editor

- Branch condition UI improvements

🎓 Why This Exists
=

This project was built to:

- Demonstrate senior-level Unity tooling design

- Show understanding of editor scripting, serialization, and UX

- Create a production-ready gameplay authoring workflow

- Serve as a portfolio and interview discussion piece

🙌 Author
=

Rushikesh Charapale

Senior Gameplay / Tools Developer

🔗 GitHub: <[rushi1962](https://github.com/rushi1962)>

🔗 LinkedIn: <[Rushikesh Charapale](https://www.linkedin.com/in/rushikesh-charapale-389288178/)>
