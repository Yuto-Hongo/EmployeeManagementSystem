import axios from "axios";

const apiClient = axios.create({
    baseURL: "http://localhost:5211/api", // 共通APIルート
    timeout: 10000, //アイムアウト(10秒)
    headers: { "Content-Type": "application/json" },
});

// リクエストインターセプター
apiClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("token");
        if (token) config.headers.Authorization = `Bearer ${token}`;
        return config;
    },
    (error) => Promise.reject(error)
);

// レスポンスインターセプタ―
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        // スキップフラグ
        if (error.config?.skipGlobalErrorHandler) {
            return Promise.reject(error)
        }

        // 共通処理: ネットワークエラー
        if (!error.response) {
            alert("ネットワークエラーが発生しました");
            return Promise.reject(error);
        } else {
            const { status } = error.response;

            switch (status) {
                case 400:
                    alert("リクエストに誤りがあります");
                    break;
                case 401:
                    alert("認証エラー: ログインしなおしてください")
                    localStorage.removeItem("token")
                    window.location.href = "/login";
                    break;
                case 403:
                    alert("アクセス権限がありません");
                    break;
                case 404:
                    alert("データが見つかりません");
                    break;
                case 500:
                    alert("サーバーエラーが発生しました")
                    break;
                default:
                    alert(`エラーが発生しました(コード: ${status})`);
            }
        }

        return Promise.reject(error);
    }
);

export default apiClient;