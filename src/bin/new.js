'use strict'
//THIS FILE WILL CHANGE NAME TO INDEX.JS WHEN THE OTHER FILE HAS BEEN REMOVED

const CommandFactory = require('../lib/commandFactory');

/*
 * Discord functionality might be moved to an initilize component later.
 * If this file gets messy it will be cleaned :)
 */
const Discord = require('../lib/discord');

const bot = new Discord();
const factory = new CommandFactory();

bot.ready(() => {
  bot.setPresence('test');
});

bot.message((message) => {
  //eslint-disable-next-line no-console
  console.log(message.content);
  if (bot.isBot(message)) return;

  const command = factory.parse(message.content);

  bot.send(message, JSON.stringify(command.schema)).
  then((res) => {
    //eslint-disable-next-line no-console
    console.log(res);
  }).
  catch(() => {
    //eslint-disable-next-line no-console
    console.error();
  })
})

bot.login();