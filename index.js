require('dotenv').config();
const Discord = require('discord.js');
const JsonHandler = require('./data/JsonHandler.js');
const Parse = require('./components/parseMessage.js');
const getRandomInt = require('./components/getRandomInt.js');
const HueController = require('./components/hue.js');
const soundCompare = require('./components/keywordCompare.js');

const client = new Discord.Client();
const data = new JsonHandler();
const parse = new Parse();
const hue = new HueController();

client.on('ready', () => {
  client.user.setPresence({ status: 'online', game: { name: 'v3.0.7', type: 'WATCHING' } });
  console.log('bot ready');
  console.log('---------------------');
});

client.on('message', async (message) => {
  try {
    // this is where the keywords with ! at the begining is handled.
    if (message.content.startsWith('!')) {

      console.log('got message: ', message.content);

      // get the keywords from the datafile, parse the message for the keyword.

      let keywords = await data.getKeywords();
      let keyword = await parse.get(message.content);
      keyword = keyword.toLowerCase();  // make the keyword lowercase

      if (keywords[keyword] === undefined) {
        console.log('given keyword is undefined, matching soundex');
        // check for similarities using soundex
        // if there is a match just set the keyword to the property from the keywords object.
        keyword = soundCompare(keyword, keywords);
      }

      if (keywords[keyword]) {
        console.log('getting random item');
        let keywordarr = keywords[keyword];         // create an array of the keyword data
        let rand = getRandomInt(keywordarr.length); // get a random int for indexing the array

        message.channel.send(keywordarr[rand]);
      }
    }

    // TODO: make the add keyword take multi line. Seems like my regex can't parse multiline.
    if (message.content.startsWith('+add')) {
      let add = await parse.add(message.content);
      
      data.addKeyword(add[1].toLowerCase(), add[2]);
      
      try {
        message.delete(5000);
      }
      catch (error) {
        console.error(error);
      }
      
      message.channel.send(add[1] + ' - ' + add[2]);
      data.getKeywords();
    }
    
    if (message.content.startsWith('+edit')) {
      let edit = await parse.edit(message.content);
      let result = await data.editKeyword(edit[1], edit[2]);
      if (result === 1) message.channel.send('editing: ' + edit[1] + ' new key: ' + edit[2]);
      else if (result === 0) message.channel.send('keywords are alike, did nothing.');
    }

    // TODO: change the way we get ligths with api.lights instead of api.getLights.
    if (message.content.startsWith('.hue')) {
      let users = await data.getUsers();

      if (users[message.author.id] === 'admin') {
        let cmd = await parse.hue(message.content);

        console.log(cmd[0]);

        if (cmd[1] == 'get') {
          let reply = await hue.getState();
          message.channel.send(reply);
        }

        if (cmd[1] == 'set') {
          try {
            hue.setState(cmd[2], state);
          }
          catch (error) {
            console.error(error);
          }
        }
      }
    }
  }
  catch (error) {
    console.error(error);
  }
});

client.login(process.env.TOKEN);