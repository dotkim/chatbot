'use strict'
//THIS FILE WILL CHANGE NAME TO INDEX.JS WHEN THE OTHER FILE HAS BEEN REMOVED

const CommandFactory = require('../api/commandFactory');
const PrefixChecker = require('../config/prefixes');
let Factory = '';

/*
 * Discord functionality might be moved to an initilize component later.
 * If this file gets messy it will be cleaned :)
 */
const Discord = require('../lib/discord');

const bot = new Discord();
const cmdfactory = new CommandFactory();

bot.ready(() => {
  bot.setPresence('test');
});

bot.message(async (message) => {
  //eslint-disable-next-line no-console
  console.log(message.content);
  if (bot.isBot(message)) return;

  const command = cmdfactory.parse(message.content);
  //eslint-disable-next-line no-unused-vars
  const aaaa = PrefixChecker(command.prefix);

  const availCmds = [
    'keyword',
    'image'
  ]

  if (availCmds.includes(aaaa)) Factory = require(`../api/${aaaa}Factory`);

  const factory = new Factory();
  let keyword = {};

  try {
    keyword = await factory.get(command.name);
  } catch (err) {
    //eslint-disable-next-line no-console
    console.error(err);
  }

  //eslint-disable-next-line no-console
  console.log(keyword);
});

bot.login();