// YY-mm-DD形式に共通変換する関数

export const formatDate = (dateString) => {
    if (!dateString) return;
    try {
        return dateString.split("T")[0];
    } catch {
        return dateString;
    }
};