// Advent of Code 2018
// Day 24

const DEBUG = false;

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(line => line.length > 0);

let immuneSystemUnits = [];
let infectionUnits = [];

function parseInput(boost) {
    const regexp = /^(\d+) units each with (\d+) hit points(?: \((.+)\))? with an attack that does (\d+) (\w+) damage at initiative (\d+)$/;
    immuneSystemUnits = [];
    infectionUnits = [];
    let currentUnits;
    lines.forEach(line => {
        if (line === 'Immune System:') {
            currentUnits = immuneSystemUnits;
        } else if (line === 'Infection:') {
            currentUnits = infectionUnits;
        } else {
            const [_, nbUnits, hitpoints, weaknessData, attack, attackType, initiative] = line.match(regexp);
            const [immunities, weaknesses] = parseWeaknessData(weaknessData);
            currentUnits.push({
                id: currentUnits.length + 1,
                team: (currentUnits === immuneSystemUnits) ? 'Immune System' : 'Infection',
                immunities,
                weaknesses,
                attackType,
                nbUnits: parseInt(nbUnits),
                hitpoints: parseInt(hitpoints),
                attack: parseInt(attack) + (currentUnits === immuneSystemUnits ? boost : 0),
                initiative: parseInt(initiative),
            });
        }
    });
}

function parseWeaknessData(weaknessData) {
    const immunities = new Set();
    const weaknesses = new Set();
    if (!weaknessData) {
        return [immunities, weaknesses];
    }
    const parts = weaknessData.split(';').map(x => x.trim());
    parts.forEach(part => {
        let elements;
        let set;
        if (part.indexOf('weak to ') === 0) {
            elements = part.substring('weak to '.length);
            set = weaknesses;
        } else if (part.indexOf('immune to ') === 0) {
            elements = part.substring('immune to '.length);
            set = immunities;
        }
        elements.split(',').map(x => x.trim()).forEach(x => set.add(x));
    });
    return [immunities, weaknesses];
}

const compareTargets = (a, b) => (b.nbUnits * b.attack * 100 + b.initiative) - (a.nbUnits * a.attack * 100 + a.initiative);

function computeDamage(attacker, defender) {
    let damage = attacker.nbUnits * attacker.attack;
    if (defender.immunities.has(attacker.attackType)) {
        damage = 0;
    }
    if (defender.weaknesses.has(attacker.attackType)) {
        damage *= 2;
    }
    return damage;
}

function targetSelectionPhase() {
    infectionUnits.sort(compareTargets);
    immuneSystemUnits.sort(compareTargets);
    targetSelectionPart(infectionUnits, immuneSystemUnits);
    targetSelectionPart(immuneSystemUnits, infectionUnits);
}

function targetSelectionPart(attackers, defenders) {
    attackers.forEach(attacker => {
        let targetedDefender = null;
        let expectedDamage = 0;
        defenders.forEach(defender => {
            let damage = computeDamage(attacker, defender);
            if (DEBUG) console.log(`${attacker.team} group ${attacker.id} would deal defending group ${defender.id} ${damage} damage`);
            if (damage > expectedDamage) {
                targetedDefender = defender;
                expectedDamage = damage;
            }
        });
        attacker.currentTarget = targetedDefender;
        defenders = defenders.filter(x => x !== targetedDefender);
    });
}

function attackPhase() {
    let didDamage = false;
    infectionUnits.concat(immuneSystemUnits).sort((a, b) => (b.initiative - a.initiative)).forEach(unit => {
        const target = unit.currentTarget;
        if (target !== null && unit.nbUnits > 0) {
            let damage = computeDamage(unit, target);
            const killedUnits = Math.min(Math.floor(damage / target.hitpoints), target.nbUnits);
            target.nbUnits -= killedUnits;
            if (killedUnits > 0) {
                didDamage = true;
            }
            if (DEBUG) console.log(`${unit.team} group ${unit.id} attacks defending group ${target.id}, killing ${killedUnits} units`);
        }
    });
    return didDamage;
}

function playTurn() {
    if (DEBUG) {
        console.log('Immune System:');
        if (immuneSystemUnits.length === 0) {
            console.log('No groups remain.');
        }
        for (const unit of immuneSystemUnits) {
            console.log(`Group ${unit.id} contains ${unit.nbUnits} units`);
        }
        console.log('Infection:');
        if (infectionUnits.length === 0) {
            console.log('No groups remain.');
        }
        for (const unit of infectionUnits) {
            console.log(`Group ${unit.id} contains ${unit.nbUnits} units`);
        }
        console.log();
    }

    if (immuneSystemUnits.length === 0 || infectionUnits.length === 0) {
        return false;
    }

    targetSelectionPhase();
    if (DEBUG) console.log();
    const didDamage = attackPhase();
    if (DEBUG) console.log();

    immuneSystemUnits = immuneSystemUnits.filter(x => x.nbUnits > 0);
    infectionUnits = infectionUnits.filter(x => x.nbUnits > 0);
    return didDamage;
}

parseInput(0);
while (playTurn()) {}
console.log(immuneSystemUnits.concat(infectionUnits).reduce((a, b) => a + b.nbUnits, 0));

for (let boost = 1; boost <= 1000; ++boost) {
    parseInput(boost);
    while (playTurn()) {}
    if (infectionUnits.reduce((a, b) => a + b.nbUnits, 0) === 0) {
        console.log(immuneSystemUnits.reduce((a, b) => a + b.nbUnits, 0));
        break;
    }
}
