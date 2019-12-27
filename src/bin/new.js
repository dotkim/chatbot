'use strict'
//THIS FILE WILL CHANGE NAME TO INDEX.JS WHEN THE OTHER FILE HAS BEEN REMOVED

/*
 * Discord functionality might be moved to an initilize component later.
 * If this file gets messy it will be cleaned :)
 */
const Discord = require('../lib/DiscordHelper');
const Worker = require('../lib/Worker');


const bot = new Discord();
bot.ready(() => {
  bot.setPresence('test');
});

const worker = new Worker(bot);
bot.message(async (message) => {
  try {
    let data = await worker.ParseMessage(message);
    data.Reply(message);
  } catch (err) {
    //eslint-disable-next-line no-console
    console.error(err);
  }
});

bot.login();