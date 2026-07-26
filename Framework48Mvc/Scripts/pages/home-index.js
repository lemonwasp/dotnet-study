// 分割代入
// Vueオブジェクトの`createApp`関数を変数にして使用する
const { createApp } = Vue;

// Vueアプリケーションを生成する
createApp({
    // 画面が使う反応型の状態を返却する
    data() {
        return {
            // 初期値を設定する
            // 後にthis.message = data.message;が実行されると、
            // Vueが変更を感知して画面を更新する
            message: '불러오는 중...',
            createdAt: ''
        };
    },

    // Vueアプリケーションが実際のDOMにマウントされた直後に
    // 呼び出されるライフサイクルフック
    // Vueアプリケーション生成 → data初期化 -> #appにマウント -> DOM準備 -> mounted()実行
    // メソッドの中でawaitを使うために、asyncを付与する
    // async関数はいつもPromiseを返す
    async mounted() {
        try {
            // ブラウザがサーバにHTTPリクエストを送る
            // デフォルトではGETメソッドで送信される
            // awaitはレスポンスが来るまで処理を一時停止する
            // しかし、ブラウザ全体が止まるわけではなく、他の処理は継続される
            // fetch()が返すのはJSONデータではなく、HTTPレスポンスオブジェクト
            const response = await fetch('/Home/GetMessage');
            // デシリアライズ
            // レスポンス本文にあるJSON文字列をJavaScriptオブジェクトに変換する
            const data = await response.json();
            // ここでthisはVueインスタンスを指す
            // サーバから貰った値をVueの反応型データに代入する
            // data.message -> this.message -> {{ message }} -> 画面更新
            this.message = data.message;
            this.createdAt = data.createdAt;
        // ネットワークエラーやJSONパーシングエラーが発生すると、
        // 開発者ツールのコンソールにエラーメッセージが表示される
        } catch (error) {
            console.error('Error fetching message:', error);
        }
    }
// VueのappをHTMLの<div id = "app">要素にマウントするために、`mount('#app')`を使用する
// これがないと、Vueアプリケーションを定義するだけで、実際にHTMLに反映されない
}).mount('#app');