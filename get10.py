"""
Get10: A daily task tracker to earn at least 10 points per day.

Features:
1. Task Management (Add/Edit/Delete tasks with point values 1-5).
2. Daily Dashboard (total points, count of 5-point tasks, streak counter).
3. Reminders & Scheduling (console reminders for high-priority tasks).

Usage:
  python get10.py run      # Start interactive CLI
  python get10.py add "Task description" --points N
  python get10.py complete ID
  python get10.py list [--filter all|pending|completed]
  python get10.py dashboard
  python get10.py remind

Data stored in 'data.json' per user/day.

"""
import json
import os
import datetime
import threading
import time
import argparse
import schedule

DATA_FILE = 'data.json'

# ----- Data Persistence -----
def load_data():
    if not os.path.exists(DATA_FILE):
        return {}
    with open(DATA_FILE, 'r') as f:
        return json.load(f)

def save_data(data):
    with open(DATA_FILE, 'w') as f:
        json.dump(data, f, indent=2)

# ----- Models & Repo -----
class Task:
    def __init__(self, id, desc, points, done=False):
        self.id = id
        self.desc = desc
        self.points = points
        self.done = done

class Get10Repo:
    def __init__(self):
        self.data = load_data()
        today = str(datetime.date.today())
        if today not in self.data:
            self.data[today] = {'tasks': [], 'streak': self.data.get('streak', 0)}
        self.today = today

    def add(self, desc, points):
        tasks = self.data[self.today]['tasks']
        next_id = max([t['id'] for t in tasks], default=0) + 1
        tasks.append({'id': next_id, 'desc': desc, 'points': points, 'done': False})
        save_data(self.data)
        return next_id

    def complete(self, tid):
        tasks = self.data[self.today]['tasks']
        for t in tasks:
            if t['id'] == tid:
                if t['done']:
                    raise ValueError('Already completed')
                t['done'] = True
                save_data(self.data)
                return t
        raise KeyError('Not found')

    def list(self, filter='all'):
        tasks = self.data[self.today]['tasks']
        if filter=='pending': return [t for t in tasks if not t['done']]
        if filter=='completed': return [t for t in tasks if t['done']]
        return tasks

    def dashboard(self):
        tasks = self.data[self.today]['tasks']
        total = sum(t['points'] for t in tasks if t['done'])
        count5 = sum(1 for t in tasks if t['done'] and t['points']==5)
        return total, count5, self.data[self.today].get('streak', 0)

    def update_streak(self):
        total, count5, streak = self.dashboard()
        if total >=10 and count5>=2:
            streak +=1
        else:
            streak =0
        self.data[self.today]['streak'] = streak
        save_data(self.data)
        return streak

# ----- CLI & Commands -----
repo = Get10Repo()
parser = argparse.ArgumentParser(prog='get10')
sub = parser.add_subparsers(dest='cmd')

add_p = sub.add_parser('add')
add_p.add_argument('desc')
add_p.add_argument('--points', type=int, choices=range(1,6), default=3)

cmp_p = sub.add_parser('complete')
cmp_p.add_argument('id', type=int)

list_p = sub.add_parser('list')
list_p.add_argument('--filter', choices=['all','pending','completed'], default='all')

dash_p = sub.add_parser('dashboard')
rem_p = sub.add_parser('remind')
run_p = sub.add_parser('run')

# ----- Reminder Scheduler -----
def start_reminders():
    tasks5 = [t for t in repo.list('pending') if t['points']==5]
    def job():
        for t in tasks5:
            print(f"[REMINDER] High-priority task pending: #{t['id']} {t['desc']}")
    schedule.every().hour.do(job)
    job()  # initial reminder
    while True:
        schedule.run_pending()
        time.sleep(60)

# ----- Main Execution -----
if __name__=='__main__':
    args = parser.parse_args()
    try:
        if args.cmd=='add':
            tid = repo.add(args.desc, args.points)
            print(f"Added #{tid} '{args.desc}' ({args.points} pts)")
        elif args.cmd=='complete':
            t = repo.complete(args.id)
            print(f"Completed #{t['id']} '{t['desc']}'")
        elif args.cmd=='list':
            for t in repo.list(args.filter):
                print(f"#{t['id']}: {t['desc']} [{t['points']} pts] - {'Done' if t['done'] else 'Pending'}")
        elif args.cmd=='dashboard':
            total, count5, streak = repo.dashboard()
            print(f"Today: {total}/10 pts, 5-pt tasks done: {count5}")
            print(f"Current streak: {streak} days")
        elif args.cmd=='remind':
            print("Starting reminders (5-pt tasks every hour)... Ctrl+C to stop.")
            start_reminders()
        elif args.cmd=='run':
            print("Interactive Get10 CLI. Type 'exit' to quit.")
            while True:
                cmd = input('> ')
                if cmd.strip().lower()=='exit': break
                os.system(f"python get10.py {cmd}")
        else:
            parser.print_help()
    except Exception as e:
        print(f"Error: {e}")
