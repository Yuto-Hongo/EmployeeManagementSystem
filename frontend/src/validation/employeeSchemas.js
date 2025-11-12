import * as yup from "yup"

// 管理権限: 従業員追加スキーマ
export const employeeAddSchema = yup.object({
    fullName: yup.string().required("氏名は必需です"),
    email: yup.string().email("正しいメールアドレスを入力してください").required("メールアドレスは必需です"),
    dateOfBirth: yup
        .date()
        .typeError("生年月日を正しい形式で入力してください")
        .required("生年月日は必需です"),
    address: yup.string().required("住所は必需です"),
    gender: yup.string().required("性別を選択してください"),
    joinDate: yup
        .date()
        .typeError("入社日を正しい形式で入力してください")
        .required("入社日は必需です"),
    currentWorkplace: yup.string().required("勤務先は必需です"),
})

// 両権限: 編集用（メールアドレスのみ）スキーマ
export const validateEmailFormat = (email) => {
    if (!email) {
        return ''; // 値が空の場合は必須チェックなし
    }
    // 標準的なメールアドレスの正規表現 
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    
    if (!emailRegex.test(email)) {
        return '無効なメールアドレス形式です。';
    }
    return ''; // バリデーション成功
};
