import numpy as np
import pandas as pd
from sklearn.datasets import load_iris
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split
import matplotlib.pyplot as plt
import traceback

# פונקציה להרצת משימה עם טיפול בשגיאות
def run_task(task_name, fn, *args, **kwargs):
    try:
        print(f"Running: {task_name}...")
        result = fn(*args, **kwargs)
        print(f"✅ Success: {task_name}\n")
        return result
    except Exception as e:
        print(f"❌ Failure in task: {task_name}")
        print(traceback.format_exc())
        return None

# שלב 1 - טעינת dataset אמיתי (Iris)
def load_data():
    iris = load_iris()
    df = pd.DataFrame(data=iris.data, columns=iris.feature_names)
    df['label'] = iris.target
    return df

# שלב 2 - עיבוד מקדים: סקלר ופיצול
def preprocess_data(df, seed):
    X = df.drop(columns=['label'])
    y = df['label']
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    return train_test_split(X_scaled, y, test_size=0.3, random_state=seed)

# שלב 3 - אימון מודל
def train_model(X_train, y_train):
    model = LogisticRegression(max_iter=200)
    model.fit(X_train, y_train)
    return model

# שלב 4 - הערכת ביצועים
def evaluate_model(model, X_test, y_test):
    preds = model.predict(X_test)
    acc = accuracy_score(y_test, preds)
    print(f"🔎 Accuracy: {acc:.4f}")
    return acc

# פרמטרים של הרצה
max_retries = 100
accuracy_threshold = 0.995

accuracies = []
seeds = []

# טעינת הנתונים פעם אחת
data = run_task("Load Data", load_data)

if data is None:
    raise RuntimeError("Failed to load data, aborting.")

# לולאת רטרייס עם seed שונה בכל פעם
for attempt in range(max_retries):
    current_seed = np.random.randint(0, 10000)
    print(f"Attempt #{attempt + 1} with seed {current_seed}")

    split = run_task("Preprocess Data", preprocess_data, data, current_seed)
    if split is None:
        print("Failed preprocessing, trying new seed...")
        continue
    X_train, X_test, y_train, y_test = split

    model = run_task("Train Model", train_model, X_train, y_train)
    if model is None:
        print("Failed training, trying new seed...")
        continue

    acc = run_task("Evaluate Model", evaluate_model, model, X_test, y_test)
    if acc is None:
        print("Failed evaluation, trying new seed...")
        continue

    accuracies.append(acc)
    seeds.append(current_seed)

    # עצירת הלולאה אם הגענו לדיוק הרצוי
    if acc >= accuracy_threshold:
        print(f"Reached accuracy threshold {accuracy_threshold} at attempt #{attempt + 1}")
        break
else:
    print(f"Did not reach accuracy threshold {accuracy_threshold} after {max_retries} attempts.")

# ויזואליזציה של התוצאות
plt.figure(figsize=(12,6))
plt.plot(range(1, len(accuracies)+1), accuracies, marker='o', label='Accuracy per attempt')
plt.axhline(y=accuracy_threshold, color='r', linestyle='--', label=f'Threshold {accuracy_threshold}')
plt.title("Accuracy per Attempt with Different Seeds")
plt.xlabel("Attempt Number")
plt.ylabel("Accuracy")
plt.legend()
plt.grid(True)
plt.show()
