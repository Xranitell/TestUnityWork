# TestUnityWork

В рамках выполнения тестового задания была разработана игра ScamBirds.

![image](https://github.com/user-attachments/assets/0c676088-3109-419b-b3b2-d77ba4b188d5)

По ТЗ выполнены следующие условия:
- Подключена FSM;
- Нажатие на старт или стоп влияют на переход между состояниями;
- Работа с кнопками через UIButtonDataBind;
- Кнопки блокируются, если нажатие на них не даст никакого результата;
- Добавлены некоторые визуальные эффекты;
- Сделан скейлинг под разные разрешения;
- Реализованы системы частиц
- Реализована центровка барабана на элементе;
- Разгон и торможение колеса;
- FSM переходит в другие состояния только внутри себя(внутри своих состояний);
- Состояния отправляют ивенты в систему и реагируют на внешние события;

  Все конфиги вынесены в ScriptableObject
![image](https://github.com/user-attachments/assets/11574004-6f3b-4b11-9fbf-ba9384d0e699)

  Все возможные предметы хранятся в ItemDatabase
![image](https://github.com/user-attachments/assets/487f4fc4-e731-42b5-8ce5-8790634db172)

  Звуки хранятся в SoundManager
![image](https://github.com/user-attachments/assets/22ac574a-ad89-46d4-a6c6-be001ff5a379)


