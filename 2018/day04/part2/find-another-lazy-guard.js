// Advent of Code 2018
// Day 04, Part 2

const fs = require('fs');
const content = fs.readFileSync('../input', 'utf8');
const lines = content.split('\n').filter(x => x !== '');

const lineRegexp = /^\[(.+)\] (.+)$/;

const entries = lines.map(line => {
    const [ _, timestring, message ] = line.match(lineRegexp);
    const date = new Date(timestring + 'Z');
    return { date, message };
});

entries.sort((a, b) => a.date - b.date);

const guardRegexp = /\#(\d+)/;

let currentGuardId = 0;
let fellAsleepAt = 0;
let guards = new Map();

for (const entry of entries) {
    if (entry.message === 'wakes up') {
        let timetable = guards.get(currentGuardId);
        if (!timetable) {
            timetable = new Array(60).fill(0)
            guards.set(currentGuardId, timetable);
        }
        const wokeUpAt = entry.date.getUTCMinutes();
        for (let i = fellAsleepAt; i < wokeUpAt; ++i) {
            timetable[i]++;
        }
    } else if (entry.message === 'falls asleep') {
        fellAsleepAt = entry.date.getUTCMinutes();
    } else {
        const [ _, guardId ] = entry.message.match(guardRegexp);
        currentGuardId = parseInt(guardId);
    }
}

let laziestGuardId = 0;
let laziestMinute = 0;
let laziestDelay = 0;

for (let i = 0; i < 60; ++i) {
    guards.forEach((timetable, id) => {
        if (timetable[i] > laziestDelay) {
            laziestGuardId = id;
            laziestMinute = i;
            laziestDelay = timetable[i];
        }
    });
}

console.log(laziestGuardId * laziestMinute);
