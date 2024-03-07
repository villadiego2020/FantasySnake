# FantasySnake
Fantasy Snake

1. การตั้งค่า Grid 
  - SizeX: _จำนวนช่องแถว X_
  - SizeY: _จำนวนช่องแภว Y_
  - Margin: _ระยะห่างระหว่าง Cell_
  - Grid Prefab: _Prefab ของ Grid_
    
![Map Config](https://github.com/villadiego2020/FantasySnake/assets/33424275/8ec7f161-d2a9-4f1b-80dd-b59bb70a44a3)

2. การตั้งค่า ข้อมูลได้แก่ Starter Hero, Collect Hero, Obstacle และ Monster
  - Starter Control Hero
    - Prefab: Prefab ของ Hero
    - Stat
      - MaxHP: ค่า HP สูงสุดของ Hero
      - HP: ค่า HP จะใช้สำหรับการเพิ่มลดเลือดในขณะเล่นเกม โดยค่า HP เริ่มต้นได้จาก ค่า MaxHP
      - Attack: ค่า Damage
      - Attack Rate: ความถี่ในการโจมตี
        
![Starter Hero Config](https://github.com/villadiego2020/FantasySnake/assets/33424275/ac4ba4e3-64d5-4438-9737-ee4bcb5b46f3)

  - Monster, และ Collect Hero ในส่วนนี้จะใช้ข้อมูลแบบเดียวกันในการตั้งค่า
    - Movment: สำหรับการเคลื่อน ในส่วนของ Monster จะไม่มีการเคลื่อนที่
    - Start Amount: จำนวนที่จะ Spawn ขึ้นมาในตอนเริ่มเกม
    - Grown Coefficient: การเติมโตของ Monster และ Hero
      - Grown Every Time: ในทุกๆ x วินาที จะทำการเพิ่มค่าความสามารถให้ Monster และ Hero
      - Grown Rate: อัตราเร่งในการเติมโต ใช้กับเวลาในการ ถ้า Rate ยิ่งเยอะ เวลาในการเติบโตจะไวขึ้น
      - Min Max HP Stat และ Min Max Attack
        - Min Max Stat: ใช้สุ่มช่วงมค่า Stat ที่จะเพิ่มขึ้น 
    - Chance Spawn List ของ โอกาสที่จะ Spawn ขึ้นมาว่าจะ Spawn กี่ตัว
      - Chance: โอกาสที่จะสุ่มเกิด
      - Amount: จำนวนที่จะ Spawn ขึ้นมา
    - Data: List ของ Hero และ Monster
   
![Collect Hero Config](https://github.com/villadiego2020/FantasySnake/assets/33424275/301b691b-aa4d-4de1-b060-ef4084daf82f)
![Monster Config](https://github.com/villadiego2020/FantasySnake/assets/33424275/b195e96e-cd2c-442c-a30e-9f013ff9052c)


3. การตั้งค่า Obstacle
  - Number of Obstacle: จำนวนการ Spawn Obstacle ตอนเริ่มเกม
  - Obstacle Datas: List ของ Obstacle ที่จะใช้ในเกม ได้แก่ Obstacle ประเภท 1x1, 1x2, 2x1, 2x2 เป็นต้น

![Obstacle Config](https://github.com/villadiego2020/FantasySnake/assets/33424275/bc5bdf25-d459-4e9f-b073-2d9a3c0f9d29)

4. Turret ป้อมยิง (จะถูกใช้งานเมื่อถึงเวลา Crazy Time)

![Turret Config](https://github.com/villadiego2020/FantasySnake/assets/33424275/78f46f07-1ef3-448b-98b2-321a40615464)

ตัวอย่าง ภาพ Gameplay

![Start Game](https://github.com/villadiego2020/FantasySnake/assets/33424275/240c6e74-f5d0-4ce0-8705-61d52e5f6dc6)
![Crazy Time](https://github.com/villadiego2020/FantasySnake/assets/33424275/1bb76591-f9de-4fd0-bfeb-298b099809c0)
![End Game](https://github.com/villadiego2020/FantasySnake/assets/33424275/50f207e7-3847-4f86-95c0-9f1c1fbd11bb)


