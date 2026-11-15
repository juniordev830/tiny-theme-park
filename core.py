"""Grid-based theme park economics with deterministic visitor simulation."""
from collections import deque
from dataclasses import dataclass


@dataclass
class Ride:
    position: tuple[int, int]
    capacity: int
    ticket: int
    upkeep: int
    opened: bool = True


class Park:
    def __init__(self, size=8, budget=500):
        if type(size) is not int or not 3 <= size <= 50:
            raise ValueError("invalid park size")
        if type(budget) is not int or budget < 0:
            raise ValueError("invalid budget")
        self.initial_budget, self.size = budget, size
        self.reset()

    def reset(self):
        self.balance = self.initial_budget
        self.paths = {(0, 0)}
        self.rides = {}
        self.day = 0

    def inside(self, at):
        return (isinstance(at, tuple) and len(at) == 2 and
                all(type(x) is int and 0 <= x < self.size for x in at))

    def add_path(self, at):
        if not self.inside(at) or at in self.rides:
            raise ValueError("invalid or occupied cell")
        self.paths.add(at)

    def build(self, at, cost, capacity=10, ticket=5, upkeep=2):
        if not self.inside(at) or at in self.paths or at in self.rides:
            raise ValueError("invalid attraction cell")
        if any(type(x) is not int or x < 0 for x in (cost, capacity, ticket, upkeep)) or capacity < 1:
            raise ValueError("invalid attraction economics")
        if cost > self.balance:
            raise ValueError("insufficient funds")
        self.balance -= cost
        self.rides[at] = Ride(at, capacity, ticket, upkeep)

    def connected_paths(self):
        seen = {(0, 0)}
        queue = deque(seen)
        while queue:
            x, y = queue.popleft()
            for neighbor in ((x+1,y),(x-1,y),(x,y+1),(x,y-1)):
                if neighbor in self.paths and neighbor not in seen:
                    seen.add(neighbor)
                    queue.append(neighbor)
        return seen

    def run_day(self, visitors: int):
        if type(visitors) is not int or visitors < 0:
            raise ValueError("invalid visitors")
        connected = self.connected_paths()
        revenue = expense = served = 0
        for ride in self.rides.values():
            expense += ride.upkeep
            x, y = ride.position
            accessible = any(p in connected for p in ((x+1,y),(x-1,y),(x,y+1),(x,y-1)))
            if ride.opened and accessible:
                count = min(ride.capacity, visitors - served)
                served += count
                revenue += count * ride.ticket
        self.balance += revenue - expense
        self.day += 1
        return {"day": self.day, "served": served, "revenue": revenue,
                "maintenance": expense, "balance": self.balance}


if __name__ == "__main__":
    park = Park()
    park.add_path((1, 0))
    park.build((2, 0), 100)
    print(park.run_day(12))
