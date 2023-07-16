import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
export default {
    install(Vue) {
        const guestHub = new Vue();
        Vue.prototype.$guestHub = guestHub;

        const connection = new HubConnectionBuilder()
            // .withUrl(`${Vue.prototype.$http.defaults.baseURL}/signalr/guest`)
            .withUrl(`http://localhost:5189/signalr/guest`)
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        connection.on('PlantAdded', (newPlant) => {
            guestHub.$emit('plant-added', { newPlant });
        });

        guestHub.plantOpened = (plantId) => {
            return connection.invoke('GetPlant', plantId);
        };

        let startedPromise = null;
        function start() {
            startedPromise = connection.start().catch((err) => {
                console.error('Failed to connect with hub', err);
                return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000));
            });
            return startedPromise;
        }
        connection.onclose(() => start());

        start();

        return connection;
    },
};
