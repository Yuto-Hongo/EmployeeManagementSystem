import Swal from "sweetalert2";

// トースト用共通設定
const Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 5000,
    timerProgressBar: true,
    background: "#fff",
    color: "#333",
    showClass: {
        popup: "swal2-show",
    },
    hideClass: {
        popup: "swal2-hide",
    },
});

// 成功メッセージ
export const showSuccess = (message = "処理が完了しました") => {
    Toast.fire({
        icon: "success",
        title: message,
    });
};

// 警告メッセージ
export const showWarning = (message = "確認が必要です") => {
    Toast.fire({
        icon: "warning",
        title: message,
    });
};

// エラーメッセージ
export const showError = (message = "エラーが発生しました") => {
    Toast.fire({
        icon: "error",
        title: message,
    });
};

// 確認ダイアログ(削除・重要操作用)
export const showConfirm = async (message = "この操作を実施しますか？") => {
    const result = await Swal.fire({
        title: "確認",
        text: message,
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "実行",
        cancelButtonText: "キャンセル",
        confirmButtonColor: "#2563eb",
        cancelButtonColor: "#6b7820",
        reverseButtons: true,
    });
    return result.isConfirmed;
}