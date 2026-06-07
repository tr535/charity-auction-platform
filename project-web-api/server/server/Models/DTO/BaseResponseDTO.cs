namespace server.Models.DTO
{
    public class BaseResponseDTO<T>
    {
        // האם הפעולה הצליחה
        public bool Success { get; set; }

        // הודעה למשתמש או לדיבאג
        public string Message { get; set; }

        // הנתונים שחוזרים (זה יכול להיות PurchaseDTO, רשימה, או null במקרה של שגיאה)
        public T Data { get; set; }

        // פונקציות עזר סטטיות כדי לקצר את הכתיבה ב-Service
        public static BaseResponseDTO<T> Ok(T data, string message = "הפעולה בוצעה בהצלחה")
            => new BaseResponseDTO<T> { Success = true, Data = data, Message = message };

        public static BaseResponseDTO<T> Failure(string message)
            => new BaseResponseDTO<T> { Success = false, Message = message, Data = default };
    }
}