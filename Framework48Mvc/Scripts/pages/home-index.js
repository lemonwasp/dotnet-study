const { createApp } = Vue;

createApp({
    data() {
        return {
            message: '불러오는 중...',
            createdAt: ''
        };
    },

    async mounted() {
        try {
            const response = await fetch('/Home/GetMessage');
            const data = await response.json();

            this.message = data.message;
            this.createdAt = data.createdAt;
        } catch (error) {
            console.error('Error fetching message:', error);
        }
    }
}).mount('#app');