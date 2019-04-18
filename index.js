require('dotenv').config();
const Discord = require('discord.js');
const JsonHandler = require('./data/JsonHandler.js');
const Parse = require('./components/parseMessage.js');
const getRandomInt = require('./components/getRandomInt.js');
const HueController = require('./components/hue.js');

const client = new Discord.Client();
const data = new JsonHandler();
const parse = new Parse();
const hue = new HueController();

client.on('ready', () => {
  client.user.setPresence({ status: 'online', game: { name: 'v3.0.1', type: 'STREAMING' } });
  console.log('bot ready');
  console.log('---------------------');
});

client.on('message', async (message) => {
  try {
    if (message.content.startsWith('!')) {
      // get the keywords from the datafile, parse the message for the keyword.
      let keywords = await data.getKeywords();
      let keyword = await parse.get(message.content);
  
      if (keywords[keyword]) {
        let keywordarr = keywords[keyword];         // create an array of the keyword data
        let rand = getRandomInt(keywordarr.length); // get a random int for indexing the array
  
        message.channel.send(keywordarr[rand]);
      }
    }
  
    if (message.content.startsWith('+add')) {
      let add = await parse.add(message.content);
      data.addKeyword(add[1], add[2]);
      try {
        message.delete(5000);
      }
      catch (error) {
        console.error(error);
      }
      message.channel.send(add[1] + ' - ' + add[2]);
    }

    if (data.users[message.author.id] === 'admin' && message.content.startsWith('.hue')) {
      let cmd = await parse.hue(message.content);
      if (cmd[0] == 'get') {
        let data = hue.getState();
        data.forEach((light) => {
          
        });
      }
    }

    
  }
  catch (error) {
    console.error(error);
  }
});

client.login(process.env.TOKEN);