"""Simple park economy simulation with capacity and budget safeguards."""
from dataclasses import dataclass, field

@dataclass
class Attraction:
    name: str
    capacity: int
    ticket_price: int
    maintenance_cost: int

    def __post_init__(self):
        if not self.name or self.capacity <= 0 or self.ticket_price < 0 or self.maintenance_cost < 0:
            raise ValueError("Invalid attraction")

@dataclass
class Park:
    balance: int = 1000
    attractions: dict[str, Attraction] = field(default_factory=dict)

    def add_attraction(self, attraction: Attraction, construction_cost: int) -> None:
        if construction_cost < 0 or attraction.name in self.attractions:
            raise ValueError("Invalid cost or duplicate attraction")
        if construction_cost > self.balance:
            raise ValueError("Insufficient funds")
        self.balance -= construction_cost
        self.attractions[attraction.name] = attraction

    def operate_day(self, visitors: dict[str, int]) -> int:
        if any(name not in self.attractions or count < 0 for name, count in visitors.items()):
            raise ValueError("Invalid visitor allocation")
        income = sum(min(visitors.get(name, 0), ride.capacity) * ride.ticket_price
                     for name, ride in self.attractions.items())
        expenses = sum(ride.maintenance_cost for ride in self.attractions.values())
        net = income - expenses
        self.balance += net
        return net
